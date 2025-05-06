export const environmentConfigs = {
    local: {
        // dotnet run --urls "http://0.0.0.0:10280"            
        baseUrl: 'http://host.docker.internal:10280',
    },
    dev: {
        baseUrl: 'https://dev.example.com',
    },
    staging: {
        baseUrl: 'https://staging.example.com',
    },
    production: {
        baseUrl: 'https://production.example.com',
    },
};

export function getEnvironmentConfig(env) {
    return environmentConfigs[env] || environmentConfigs.dev;
}