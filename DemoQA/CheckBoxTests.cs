using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;
using NUnit.Framework;

namespace DemoQA
{
    public class CheckBoxTests : PageTest
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
        public async Task CheckCheckBoxUrl()
        {
            await Page.ClickAsync("text=Check Box");
            await Expect(Page).ToHaveURLAsync("https://demoqa.com/checkbox");
        }

        [Test]
        public async Task CheckAllItem()
        {
            await Page.ClickAsync("text=Check Box");
            await Page.CheckAsync(".rct-checkbox");

            await Expect(Page.Locator("#result")).ToContainTextAsync("You have selected :homedesktopnotescommandsdocumentsworkspacereactangularveuofficepublicprivateclassifiedgeneraldownloadswordFileexcelFile");
        }

        [Test]
        public async Task CheckExpandCollapsAll(){
            await Page.ClickAsync("text=Check Box");
            await Page.GetByTitle("Expand all").ClickAsync();
            await Expect(Page.Locator(".rct-checkbox")).ToHaveCountAsync(17);
            await Page.GetByTitle("Collapse all").ClickAsync();
            await Expect(Page.Locator(".rct-checkbox")).ToHaveCountAsync(1);
        }

        [Test]
        public async Task CheckedReact(){
            await Page.ClickAsync("text=Check Box");
            await Page.GetByTitle("Expand all").ClickAsync();
            // await Page.Locator("tree-node-react").ClickAsync(); //TODO: do click on checkbox

            // await Expect(Page.GetByText("text=React")).ToBeCheckedAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await Page.CloseAsync();
        }
    }
}