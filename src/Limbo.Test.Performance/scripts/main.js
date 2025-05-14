import { smokeTest } from './scenarios/smoketest.js';
import { loadTest } from './scenarios/loadtest.js';
import { soakTest } from './scenarios/soaktest.js';
import { breakpointTest } from './scenarios/breakpointtest.js';
import { spikeTest } from './scenarios/spiketest.js';
import { averageTest } from './scenarios/averagetest.js';

import { scenarioConfigs } from './config/scenarios.js';
import { getEnvironmentConfig } from './config/environment.js';
import { thresholds } from './config/thresholds.js';

const VALID_TEST_TYPES = ['smoke', 'load', 'average', 'spike', 'soak', 'breakpoint'];
const TEST_ENV = __ENV.TEST_ENV || 'dev';
const TEST_TYPES = (__ENV.TEST_TYPES || 'smoke,load')
  .split(',')
  .map((s) => s.trim())
  .filter((testType) => VALID_TEST_TYPES.includes(testType));

const envConfig = getEnvironmentConfig(TEST_ENV);
__ENV.BASE_URL = envConfig.baseUrl;

let scenarios = {};
TEST_TYPES.forEach((testType) => {
  if (scenarioConfigs[testType]) {
    scenarios[testType] = scenarioConfigs[testType];
  }
});

export let options = {
  scenarios,
  thresholds,
};

export { averageTest, spikeTest, smokeTest, loadTest, soakTest, breakpointTest };

// To run locally using docker run following command:
// docker compose run --rm k6 run --env TEST_ENV=local --env TEST_TYPES=smoke,load --env BASE_URL=http://host.docker.internal:10280 /scripts/milan.js
