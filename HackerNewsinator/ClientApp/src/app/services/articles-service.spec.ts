import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ArticlesService } from './articles-service';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

describe('ArticlesService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [
      { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
      ArticlesService,
    ]
  }));

  it('should be created', () => {
    const service: ArticlesService = TestBed.get(ArticlesService);
    expect(service).toBeTruthy();
  });
});
