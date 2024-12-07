import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import { fileURLToPath, URL } from 'url';
import { createRequire } from 'module';

const require = createRequire(import.meta.url);
const sass = require('sass');

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd());

  return {
    plugins: [vue()],
    css: {
      preprocessorOptions: {
        scss: {
          api: 'modern',
        },
      },
    },
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
        '@app': fileURLToPath(new URL('./src/modules/app', import.meta.url)),
      },
    },
    define: {
      'process.env': env,
    },
    server: {
      port: parseInt(env.VITE_PORT) || 3000, 
      https: env.VITE_HTTPS === 'true', 
      watch: {
        ignored: ['**/.env.local']
      }
    }
  };
});
