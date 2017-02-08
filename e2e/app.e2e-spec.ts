import { SocialCopsPage } from './app.po';

describe('social-cops App', function() {
  let page: SocialCopsPage;

  beforeEach(() => {
    page = new SocialCopsPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
