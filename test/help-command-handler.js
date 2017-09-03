'use strict';

const assert = require('chai').assert;

const HelpCommandHandler = require('../lib/handlers/help-command-handler');

describe('HelpCommandHandler', () => {
  describe('#handle()', () => {
    it('should return general help message when the command data is null', () => {
      return new HelpCommandHandler(null).handle().then(response => {
        assert.include(response, 'The following commands are available');
      });
    });
    it('should return general help message when command data is not known', () => {
      return new HelpCommandHandler('something').handle().then(response => {
        assert.include(response, 'The following commands are available');
      });

    });
    it('should return detailed help message when command data is known', () => {
      return new HelpCommandHandler('hi').handle().then(response => {
        assert.include(response, 'The *hi* command will return a friendly hello.');
      });
    });
  });
});

