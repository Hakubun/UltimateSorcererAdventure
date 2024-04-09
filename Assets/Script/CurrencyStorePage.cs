using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class CurrencyStorePage : MonoBehaviour, IStoreListener
{
    [SerializeField] private UIProduct UIProductPrefab_gold;
    [SerializeField] private UIProduct UIProductPrefab_gem;
    [SerializeField] private GridLayoutGroup ContentPanel;
    [SerializeField] private GameObject LoadingOverlay;
    [SerializeField] private GameObject LoadingIcons;
    [SerializeField] private StatusBarUI StatusBar;
    [SerializeField] private AudioSource SuccessSound;
    [SerializeField] private AudioSource FailedSound;
    [SerializeField] private AudioSource ErrorSound;
    [SerializeField] private GameObject ErrorPopUp;
    [SerializeField] private TextMeshProUGUI errorText;
    private bool IconsLoaded;


    private Action OnPurchaseCompleted;
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;

    private void OnEnable()
    {
        if (!IconsLoaded)
        {

            LoadingIcons.SetActive(true);
        }
    }
    [Obsolete]
    private async void Awake()
    {
        IconsLoaded = false;
        //DontDestroyOnLoad(this.gameObject);
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPCatalogLoaded;
    }

    [Obsolete]
    private void HandleIAPCatalogLoaded(AsyncOperation Operation)
    {
        ResourceRequest request = Operation as ResourceRequest;

        Debug.Log($"Asset: {request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);

        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#endif

#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.GooglePlay)
        );
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore)
        );
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified)
        );
#endif
        foreach (ProductCatalogItem item in catalog.allProducts)
        {

            builder.AddProduct(item.id, item.type);
        }

        UnityPurchasing.Initialize(this, builder);

    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("CurrencyPageScriptOnInitialized");
        StoreController = controller;
        ExtensionProvider = extensions;
        Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {StoreController.products.all.Length} products");

        // Remove this when pushing publishing build
        // errorText.text = $"Successfully Initialized Unity IAP. Store Controller has {StoreController.products.all.Length} products";
        // ErrorPopUp.SetActive(true);


        StoreIconProvider.Initialize(StoreController.products);
        StoreIconProvider.OnLoadComplete += HandleAllIconsLoaded;

    }

    private void HandleAllIconsLoaded()
    {
        //Disable Loading
        LoadingIcons.SetActive(false);
        IconsLoaded = true;
        StartCoroutine(CreateUI());

    }

    private IEnumerator CreateUI()
    {
        List<Product> sortedProducts = StoreController.products.all.TakeWhile(item => !item.definition.id.Contains("sale")).OrderBy(item => item.metadata.localizedPrice).ToList();



        foreach (Product product in sortedProducts)
        {
            if (product.definition.id.Contains("gold"))
            {

                UIProduct uiProduct = Instantiate(UIProductPrefab_gold);
                uiProduct.OnPurchase += HandlePurchase;
                uiProduct.Setup(product);
                uiProduct.transform.SetParent(ContentPanel.transform, false);
            }
            else if (product.definition.id.Contains("gem"))
            {
                UIProduct uiProduct = Instantiate(UIProductPrefab_gem);
                uiProduct.OnPurchase += HandlePurchase;
                uiProduct.Setup(product);
                uiProduct.transform.SetParent(ContentPanel.transform, false);

            }


            yield return null;
        }

        // GridLayoutGroup group = ContentPanel.GetComponent<GridLayoutGroup>();
        // float spacing = group.spacing;
        // float horizontalPadding = group.padding.left + group.padding.right;
        // float itemsize = ContentPanel_Gem.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

        // RectTransform contentPanelRectTransform = ContentPanel_Gem.GetComponent<RectTransform>();
        // contentPanelRectTransform.sizeDelta = new(horizontalPadding + (spacing + itemsize) * sortedProducts.Count, contentPanelRectTransform.sizeDelta.y);
    }

    private void HandlePurchase(Product Product, Action OnPurchaseCompleted)
    {
        LoadingOverlay.SetActive(true);
        this.OnPurchaseCompleted = OnPurchaseCompleted;
        StoreController.InitiatePurchase(Product);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error initializing IAP because of {error}. \r\nShow a message to the player depending on the error");
        ErrorSound.Play();
        errorText.text = error.ToString();
        ErrorPopUp.SetActive(true);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP because of {error}. \r\n{message}");
        ErrorSound.Play();
        errorText.text = error.ToString() + $" Deatil: {message}";
        ErrorPopUp.SetActive(true);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
        FailedSound.Play();
        LoadingOverlay.SetActive(false);
        errorText.text = failureReason.ToString();
        ErrorPopUp.SetActive(true);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;


        //do something like give gold or something
        if (purchaseEvent.purchasedProduct.definition.id.Contains("gold"))
        {
            _ = AddGoldToPlayerAccountAsync(purchaseEvent.purchasedProduct);


        }
        else if (purchaseEvent.purchasedProduct.definition.id.Contains("gem"))
        {
            _ = AddGemsToPlayerAccountAsync(purchaseEvent.purchasedProduct);
        }

        SuccessSound.Play();
        LoadingOverlay.SetActive(false);
        return PurchaseProcessingResult.Complete;
    }

    void OnDestroy()
    {
        StoreIconProvider.OnLoadComplete -= HandleAllIconsLoaded;
        // Unsubscribe from any other events here
    }

    #region Payout
    private async Task AddGoldToPlayerAccountAsync(Product product)
    {
        // Determine the amount of gold to add based on the product
        // This might involve looking up the product's metadata or having predefined values
        int goldAmount = DetermineGoldAmount(product);
        int coinNum = await SaveSystem.LoadCoin();
        coinNum += goldAmount;
        SaveSystem.SaveCoin(coinNum);
        StatusBar.UpdateCoin();
        // Add gold to the player's account
        // UpdatePlayerGoldBalance is a hypothetical method you would implement
        //UpdatePlayerGoldBalance(goldAmount);
    }

    private async Task AddGemsToPlayerAccountAsync(Product product)
    {
        // Similar to AddGoldToPlayerAccount, but for gems
        int gemAmount = DetermineGemAmount(product);
        int gemNum = await SaveSystem.LoadGem();
        gemNum += gemAmount;
        SaveSystem.SaveGem(gemNum);
        StatusBar.UpdateGem();
        //UpdatePlayerGemBalance(gemAmount);
    }

    // Example methods to determine the amount of currency to add
    private int DetermineGoldAmount(Product product)
    {
        int amount = 0;
        // Implement logic to determine how much gold this product gives
        switch (product.definition.id)
        {
            case "gold240":
                amount = 240;
                break;
            case "gold1500":
                amount = 1500;
                break;
            case "gold8000":
                amount = 8000;
                break;
            case "gold15000":
                amount = 15000;
                break;

        }
        return amount; // Example value
    }

    private int DetermineGemAmount(Product product)
    {
        int amount = 0;
        // Implement logic to determine how many gems this product gives
        switch (product.definition.id)
        {
            case "gem180":
                amount = 180;
                break;
            case "gem1200":
                amount = 1200;
                break;
            case "gem6500":
                amount = 6500;
                break;
            case "gem14000":
                amount = 14000;
                break;

        }
        return amount; // Example value
    }

    #endregion
}
