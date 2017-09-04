'use strict';

const assert = require('chai').assert;

const CommandFactory = require('../lib/command-factory');

describe('CommandFactory', () => {

  describe('#getCommand()', () => {

    it('should return null when the command name is not known', () => {
      const command = CommandFactory.getCommand('unknown', null);
      assert.equal(command, null);
    });

    it('should return handler when command name is known', () => {
      const command = CommandFactory.getCommand('help', null);
      assert.isOk(command);
    });
  });
});

