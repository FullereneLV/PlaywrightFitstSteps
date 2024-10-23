using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;
using NUnit.Framework;

namespace DemoQA
{
    public class TextBoxTests : PageTest
    {
        private IPage Page {get; set;}

        [SetUp]
        public async Task SetUp(){
            var playwright = await CreateAsync();
            playwright.Selectors.SetTestIdAttribute("aria-label");
            var browser =  await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{
                Headless= false
            });
            Page = await browser.NewPageAsync();
            await Page.GotoAsync("https://demoqa.com/elements");
        }

        [Test]
        public async Task CheckUrl()
        {
            await Page.ClickAsync("text=Text Box");
            await Expect(Page).ToHaveURLAsync("https://demoqa.com/text-box");
        }

        [Test]
        public async Task CheckElementIsVisible(){
            await Page.ClickAsync("text=Text Box");
            await Expect(Page.Locator("h1.text-center")).ToHaveTextAsync("Text Box");
            await Expect(Page.Locator("h1.text-center")).ToBeVisibleAsync();
            await Expect(Page.Locator("#userName-label")).ToBeVisibleAsync();
            await Expect(Page.Locator("#userEmail-label")).ToBeVisibleAsync();
            await Expect(Page.Locator("#currentAddress-label")).ToBeVisibleAsync();
            await Expect(Page.Locator("#permanentAddress-label")).ToBeVisibleAsync();
            await Expect(Page.Locator("#userName")).ToBeVisibleAsync();
            await Expect(Page.Locator("#userEmail")).ToBeVisibleAsync();
            await Expect(Page.Locator("#currentAddress")).ToBeVisibleAsync();
            await Expect(Page.Locator("#permanentAddress")).ToBeVisibleAsync();
            await Expect(Page.Locator("#output")).ToBeHiddenAsync();
        }

        [Test]
        public async Task AddInfoToFields()
        {
            await Page.ClickAsync("text=Text Box");
            await Page.Locator("#userName").FillAsync("Nick Feber");
            await Page.Locator("#userEmail").FillAsync("Nick.Feber@google.com");
            await Page.Locator("#currentAddress").FillAsync("NW, City Town 2502");
            await Page.Locator("#permanentAddress").FillAsync("Canada");
            await Expect(Page.Locator("#output")).ToBeHiddenAsync();

            await Page.Locator("#submit").ClickAsync();

            await Expect(Page.Locator("#output")).ToBeVisibleAsync();
            await Expect(Page.Locator("h1.text-center")).ToBeVisibleAsync();
            await Expect(Page.Locator("#output")).ToContainTextAsync("Nick Feber");
            await Expect(Page.Locator("#output")).ToContainTextAsync("Nick.Feber@google.com");
            await Expect(Page.Locator("#output")).ToContainTextAsync("NW, City Town 2502");
            await Expect(Page.Locator("#output")).ToContainTextAsync("Canada");
        }

        [Test]
        public async Task IncorrectEmail(){
            await Page.ClickAsync("text=Text Box");
            await Page.Locator("#userName").FillAsync("Nick Feber");
            await Page.Locator("#userEmail").FillAsync("Nick.Febergooglecom");
            await Page.Locator("#currentAddress").FillAsync("NW, City Town 2502");
            await Page.Locator("#permanentAddress").FillAsync("Canada");
            await Page.Locator("#submit").ClickAsync();

            await Expect(Page.Locator("#output")).ToBeHiddenAsync();
            await Expect(Page.Locator("#userEmail.mr-sm-2.field-error.form-control")).ToBeVisibleAsync();
            await Expect(Page.Locator("#userEmail")).ToHaveCSSAsync("border", "1px solid rgb(255, 0, 0)");
        }

        [TearDown]
        public async Task TearDown()
        {
            await Page.CloseAsync();
        }
    }
}