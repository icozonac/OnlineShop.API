using OnlineShop.API.Models;

namespace OnlineShop.API.Controllers.DataAccess
{
    public interface IDataAccess
    {
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategory(int id);
        Offer GetOffer(int id);
        List<Product> GetProducts(string category, string subcategory, int count);
        Product GetProduct(int id);
        bool InsertUser(User user);
        string IsUserPresent(string email, string password);
        void InsertReview(Review review);
        List<Review> GetProductReviews(int productId);
        User GetUser(int id);
        bool InsertCartItem(int userId, int productId);
        Cart GetActiveCartOfUser(int userId);
        Cart GetCart(int cartId);
        List<Cart> GetAllPreviousCartsOfUser(int userId);



    }
}
