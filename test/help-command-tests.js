'use strict';

const assert = require('chai').assert;

const HelpCommand = require('../lib/commands/help-command');

describe('HelpCommand', () => {

  describe('#handle()', () => {

    it('should return general help message when the command data is null', () => {
      return new HelpCommand(null).handle().then(response => {
        assert.include(response, 'The following commands are available');
      });
    });

    it('should return general help message when command data is not known', () => {
      return new HelpCommand('something').handle().then(response => {
        assert.include(response, 'The following commands are available');
      });
    });

    it('should return detailed help message when command data is known', () => {
      return new HelpCommand('hi').handle().then(response => {
        assert.include(response, 'The *hi* command will return a friendly hello.');
      });
    });
  });
});

