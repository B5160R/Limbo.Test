import http from 'k6/http';
import { check, fail } from 'k6';
import { endpoints } from '../config/endpoints.js';

export function smokeTest() {
  let allPassed = true;

  for (const path of endpoints.smoke) {
    const res = http.get(`${__ENV.BASE_URL}${path}`);
    const ok = check(res, {
      [`${path} status 200`]: (r) => r.status === 200,
    });

    if (!ok) {
      allPassed = false;
      console.error(`❌ ${path} failed`);
    }
  }

  if (!allPassed) fail('Some smoke test endpoints failed');
}
