'use strict';

const assert = require('chai').assert;

const CommandHandlerFactory = require('../lib/command-handler-factory');

describe('CommandHandlerFactory', () => {
  describe('#getHandler()', () => {
    it('should return null when the command name is not known', () => {
      const handler = CommandHandlerFactory.getHandler('unknown', null);
      assert.equal(handler, null);
    });
    it('should return handler when command name is known', () => {
      const handler = CommandHandlerFactory.getHandler('help', null);
      assert.isOk(handler);
    });
  });
});

