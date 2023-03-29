const path = require('path');
const CopyPlugin = require('copy-webpack-plugin');
module.exports = function override(config, env) {
  config.plugins.push(
    new CopyPlugin({
      patterns: [
        {
          from: 'models/**/*',
          context: path.dirname(require.resolve('@0xalter/mocap4face/package.json')),
        },
        {
          from: '*.json',
          context: path.dirname(require.resolve('@0xalter/mocap4face/package.json')),
        },
      ],
    })
  );
  return config;
};
