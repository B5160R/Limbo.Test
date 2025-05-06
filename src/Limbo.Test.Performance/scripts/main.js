import { smokeTest } from './scenarios/smoketest.js';
import { loadTest } from './scenarios/loadtest.js';
import { soakTest } from './scenarios/soaktest.js';
import { breakpointTest } from './scenarios/breakpointtest.js';
import { spikeTest } from './scenarios/spiketest.js';
import { averageTest } from './scenarios/averagetest.js';

const TEST_ENV = __ENV.TEST_ENV || 'dev';
const TEST_TYPES = (__ENV.TEST_TYPES || 'smoke,load').split(',').map(s => s.trim());

let scenarios = {};

if (TEST_TYPES.includes('smoke')) {
    scenarios.smoke = {
        executor: 'shared-iterations',
        vus: 1,
        iterations: 1,
        exec: 'smokeTest',
    };
}

if (TEST_TYPES.includes('load')) {
    scenarios.load = {
        executor: 'constant-vus',
        vus: 10,
        duration: '30s',
        exec: 'loadTest',
        startTime: '3s',
    };
}

if (TEST_TYPES.includes('average') && TEST_ENV === 'staging') {
    scenarios.average = {
        executor: 'constant-arrival-rate',
        rate: 5,               // 5 requests per second
        timeUnit: '1s',
        duration: '1m',
        preAllocatedVUs: 10,
        maxVUs: 50,
        exec: 'averageTest',
        startTime: '60s'
    }
}


if (TEST_TYPES.includes('spike') && TEST_ENV === 'staging') {
    scenarios.spike = {
        executor: 'ramping-arrival-rate',
        startRate: 0,
        timeUnit: '1s',
        preAllocatedVUs: 50,
        maxVUs: 200,
        stages: [
            { target: 100, duration: '10s' }, // ramp to spike
            { target: 100, duration: '30s' }, // sustain spike
            { target: 0, duration: '10s' },   // drop off
        ],
        exec: 'spikeTest',
        startTime: '90s'
    }
}

if (TEST_TYPES.includes('soak') && TEST_ENV === 'staging') {
    scenarios.soak = {
        executor: 'constant-vus',
        vus: 5,
        duration: '10m',
        exec: 'soakTest',
        startTime: '40s',
    };
}

if (TEST_TYPES.includes('breakpoint') && TEST_ENV === 'staging') {
    scenarios.breakpoint = {
        executor: 'ramping-vus',
        startVUs: 1,
        stages: [
            { duration: '2m', target: 10 },
            { duration: '2m', target: 20 },
            { duration: '2m', target: 30 },
        ],
        exec: 'breakpointTest',
        startTime: '12m',
    };
}

export let options = { scenarios };
export { averageTest, spikeTest, smokeTest, loadTest, soakTest, breakpointTest };

// TEST_ENV=staging BASE_URL=https://staging.example.com k6 run tests/main.js

