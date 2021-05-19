import { AppPage } from './app.po';
import { browser, by, element, ExpectedConditions } from 'protractor';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should show loading indicator', () => {
    page.navigateTo();
    var EC = ExpectedConditions;
    browser.wait(EC.presenceOf(page.getLoadingIndicator()), 1000, "Loading indicator not present");
  });

  it('should show error indicator', () => {
    page.navigateTo();
    var EC = ExpectedConditions;
    browser.wait(EC.presenceOf(page.getErrorIndicator()), 5000, "Loading indicator not present");
  });

  it ('should navigate with the router to About', () => {
    page.navigateToAbout();
    expect(page.getMainHeaderText()).toEqual('About');
  });
});
