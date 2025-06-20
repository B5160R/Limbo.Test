// tests/config/endpoints.js
export const endpoints = {
  smoke: [
    '/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/limbotestarea/test-underforside/alle-block-elementer/&parts=content,site',
    '/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/jobliste/&parts=content,site',
  ],
  load: [
    '/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/jobliste/&parts=content,site',
    '/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/limbotestarea/test-underforside/alle-block-elementer/&parts=content,site',
    // '/api/pages/home',
    // '/api/pages/about',
    // '/api/posts',
    // '/api/posts/my-latest-post'
  ],
  soak: [
    '/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/jobliste/&parts=content,site',
    // '/api/pages/long-form-content',
    // '/api/posts?tag=popular'
  ],
  breakpoint: [
    // '/api/search?q=heavy+query'
    '/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/jobliste/&parts=content,site',
  ]
};
