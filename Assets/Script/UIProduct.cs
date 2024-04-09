using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using System;


public class UIProduct : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI DescriptionText;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI PriceText;
    [SerializeField] private Button PurchaseButton;

    public delegate void PurchaseEvent(Product Model, Action OnComplete);
    public event PurchaseEvent OnPurchase;
    private Product Model;
    private string ProductName;

    public void Setup(Product Product)
    {
        Model = Product;
        NameText.SetText(ProductNameCleanUp(Product));
        DescriptionText.SetText(Product.metadata.localizedDescription);
        PriceText.SetText($"{Product.metadata.localizedPriceString} " + $"{Product.metadata.isoCurrencyCode}");
        Texture2D texture = StoreIconProvider.GetIcon(Product.definition.id);
        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), Vector2.one / 2f);

            Icon.sprite = sprite;
        }
        else 
        {
            Debug.LogError($"No Sprite found for {Product.definition.id}!");
        }
    }

    public void Purchase()
    {
        PurchaseButton.enabled = false;
        OnPurchase?.Invoke(Model, HandlePurchaseComplete);
    }

    private void HandlePurchaseComplete()
    {
        PurchaseButton.enabled = true;
    }

    private string ProductNameCleanUp(Product _product)
    {
        string name = _product.metadata.localizedTitle;
        string[] parts = name.Split(new string[] { " (" }, StringSplitOptions.None);

        return parts[0];
    }
}
