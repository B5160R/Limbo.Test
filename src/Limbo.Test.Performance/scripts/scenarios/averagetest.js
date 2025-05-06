import http from 'k6/http';
import { check, sleep } from 'k6';
import { endpoints } from '../config/endpoints.js';

export function averageTest() {
  const url = endpoints.average[Math.floor(Math.random() * endpoints.average.length)];
  const res = http.get(`${__ENV.BASE_URL}${url}`);
  check(res, {
    'status 200': (r) => r.status === 200,
    'duration < 300ms': (r) => r.timings.duration < 300,
  });
  sleep(0.5);
}