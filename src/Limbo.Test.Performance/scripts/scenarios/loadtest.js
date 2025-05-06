import http from 'k6/http';
import { check, sleep } from 'k6';
import { endpoints } from '../config/endpoints.js';

export function loadTest() {
  const paths = endpoints.load;

  const path = paths[Math.floor(Math.random() * paths.length)];
  const res = http.get(`${__ENV.BASE_URL}${path}`);
  check(res, {
    [`${path} is OK`]: (r) => r.status === 200,
    'fast enough (<300ms)': (r) => r.timings.duration < 300,
  });

  sleep(1);
}