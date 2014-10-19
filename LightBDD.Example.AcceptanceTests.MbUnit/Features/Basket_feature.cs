using MbUnit.Framework;

namespace LightBDD.Example.AcceptanceTests.MbUnit.Features
{
    [Description(
@"In order to buy products
As a customer
I want to add products to basket")]
    [TestFixture]
    [Label("Story-4")]
    public partial class Basket_feature
    {
        [Test]
        [Label("Ticket-6")]
        public void No_product_in_stock()
        {
            Runner.RunScenario(
                Given_product_is_out_of_stock,
                When_customer_adds_it_to_basket,
                Then_product_addition_is_unsuccessful,
                Then_basket_does_not_contain_product);
        }

        /// <summary>
        /// This test presents how LightBDD treats tests with Inconclusive / Ignore asserts
        /// </summary>
        [Test]
        [Label("Ticket-7")]
        public void Successful_addition()
        {
            Runner.RunScenario(
                Given_product_is_in_stock,
                When_customer_adds_it_to_basket,
                Then_product_addition_is_successful,
                Then_basket_contains_product,
                Then_product_is_removed_from_stock);
        }
    }
}