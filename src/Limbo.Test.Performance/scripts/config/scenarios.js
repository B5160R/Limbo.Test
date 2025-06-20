export const scenarioConfigs = {
  smoke: {
    executor: 'shared-iterations',
    vus: 1,
    iterations: 1,
    exec: 'smokeTest',
  },
  load: {
    executor: 'constant-vus',
    vus: 10,
    duration: '30s',
    exec: 'loadTest',
    startTime: '3s',
  },
  average: {
    executor: 'constant-arrival-rate',
    rate: 5,
    timeUnit: '1s',
    duration: '1m',
    preAllocatedVUs: 10,
    maxVUs: 50,
    exec: 'averageTest',
    startTime: '60s',
  },
  spike: {
    executor: 'ramping-arrival-rate',
    startRate: 0,
    timeUnit: '1s',
    preAllocatedVUs: 50,
    maxVUs: 200,
    stages: [
      { target: 100, duration: '10s' },
      { target: 100, duration: '30s' },
      { target: 0, duration: '10s' },
    ],
    exec: 'spikeTest',
    startTime: '90s',
  },
  soak: {
    executor: 'constant-vus',
    vus: 5,
    duration: '10m',
    exec: 'soakTest',
    startTime: '40s',
  },
  breakpoint: {
    executor: 'ramping-vus',
    startVUs: 1,
    stages: [
      { duration: '2m', target: 10 },
      { duration: '2m', target: 20 },
      { duration: '2m', target: 30 },
    ],
    exec: 'breakpointTest',
    startTime: '12m',
  },
};