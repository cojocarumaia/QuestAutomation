using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestAutomation
{
    public class Tests : PageTest
    {
        private async Task RemoveConsentOverlayIfPresent()
        {
            await Page.EvaluateAsync(@"() => {
                const overlay = document.querySelector('.fc-consent-root');
                if (overlay) overlay.remove();
            }");
        }

        [Test]
        public async Task InvalidLoginTest()
        {
            await Page.GotoAsync("https://automationexercise.com/login");
            await RemoveConsentOverlayIfPresent();

            await Page.FillAsync("input[data-qa='login-email']", "test@test.com");
            await Page.FillAsync("input[data-qa='login-password']", "123456");
            await Page.ClickAsync("button[data-qa='login-button']", new() { Force = true });

            await Expect(Page.Locator("body"))
                .ToContainTextAsync("Your email or password is incorrect!");
        }

        [Test]
        public async Task OpenTestCasesPageTest()
        {
            await Page.GotoAsync("https://automationexercise.com/test_cases");
            await RemoveConsentOverlayIfPresent();

            await Expect(Page).ToHaveURLAsync(new Regex("test_cases"));
            await Expect(Page.Locator("body"))
                .ToContainTextAsync("Test Cases");
        }

        [Test]
        public async Task ProductsAndProductDetailPageTest()
        {
            await Page.GotoAsync("https://automationexercise.com/products");
            await RemoveConsentOverlayIfPresent();

            await Expect(Page.Locator(".features_items")).ToBeVisibleAsync();

            await Page.GotoAsync("https://automationexercise.com/product_details/1");
            await RemoveConsentOverlayIfPresent();

            await Expect(Page.Locator(".product-information")).ToBeVisibleAsync();
        }

        [Test]
        public async Task SearchProductTest()
        {
            await Page.GotoAsync("https://automationexercise.com/products");
            await RemoveConsentOverlayIfPresent();

            await Page.FillAsync("#search_product", "Dress");
            await Page.ClickAsync("#submit_search", new() { Force = true });

            await Expect(Page.Locator(".product-image-wrapper").First)
                .ToBeVisibleAsync();
        }

        [Test]
        public async Task HomePageTest()
        {
            await Page.GotoAsync("https://automationexercise.com/");
            await RemoveConsentOverlayIfPresent();

            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Page.FillAsync("#susbscribe_email", "test1@mail.com");
            await Page.ClickAsync("#subscribe", new() { Force = true });

            await Expect(Page.Locator("#success-subscribe"))
                .ToContainTextAsync("You have been successfully subscribed!");
        }

        [Test]
        public async Task CartPageTest()
        {
            await Page.GotoAsync("https://automationexercise.com/view_cart");
            await RemoveConsentOverlayIfPresent();

            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Page.FillAsync("#susbscribe_email", "test2@mail.com");
            await Page.ClickAsync("#subscribe", new() { Force = true });

            await Expect(Page.Locator("#success-subscribe"))
                .ToContainTextAsync("You have been successfully subscribed!");
        }

        [Test]
        public async Task OpenCategoryProductsPageTest()
        {
            await Page.GotoAsync("https://automationexercise.com/category_products/1");
            await RemoveConsentOverlayIfPresent();

            await Expect(Page).ToHaveURLAsync(new Regex("category_products"));
            await Expect(Page.Locator(".features_items")).ToBeVisibleAsync();
        }

        [Test]
        public async Task OpenBrandProductsPageTest()
        {
            await Page.GotoAsync("https://automationexercise.com/brand_products/Polo");
            await RemoveConsentOverlayIfPresent();

            await Expect(Page).ToHaveURLAsync(new Regex("brand_products"));
            await Expect(Page.Locator("body"))
                .ToContainTextAsync("Polo");
        }

        [Test]
        public async Task ScrollUpUsingArrowButtonTest()
        {
            await Page.GotoAsync("https://automationexercise.com/");
            await RemoveConsentOverlayIfPresent();

            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Page.ClickAsync("#scrollUp", new() { Force = true });

            await Expect(Page.Locator("#slider")).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScrollUpWithoutArrowButtonTest()
        {
            await Page.GotoAsync("https://automationexercise.com/");
            await RemoveConsentOverlayIfPresent();

            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Page.EvaluateAsync("window.scrollTo(0, 0)");

            await Expect(Page.Locator("#slider")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Additional_LoginWithEmptyCredentialsTest()
        {
            await Page.GotoAsync("https://automationexercise.com/login");
            await RemoveConsentOverlayIfPresent();

            await Page.FillAsync("input[data-qa='login-email']", "");
            await Page.FillAsync("input[data-qa='login-password']", "");
            await Page.ClickAsync("button[data-qa='login-button']", new() { Force = true });

            await Expect(Page).ToHaveURLAsync(new Regex("login"));
        }

    }
}