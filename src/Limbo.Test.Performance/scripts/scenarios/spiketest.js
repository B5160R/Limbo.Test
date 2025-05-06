import http from 'k6/http';
import { check } from 'k6';
import { endpoints } from '../config/endpoints.js';

export function spikeTest() {
  const url = endpoints.spike[Math.floor(Math.random() * endpoints.spike.length)];
  const res = http.get(`${__ENV.BASE_URL}${url}`);
  check(res, {
    'status 200': (r) => r.status === 200,
    'duration < 500ms': (r) => r.timings.duration < 500,
  });
}