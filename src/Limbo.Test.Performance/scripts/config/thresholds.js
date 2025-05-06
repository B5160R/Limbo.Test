export const thresholds = {
  http_req_duration: ['p(95)<500'], // 95% of requests should complete within 500ms
  http_req_failed: ['rate<0.01'],  // Less than 1% of requests should fail
};