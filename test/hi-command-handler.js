'use strict';

const assert = require('chai').assert;

const HiCommandHandler = require('../lib/handlers/hi-command-handler');

describe('HiCommandHandler', () => {
  describe('#handle()', () => {
    it('should return response when the command data is null', () => {
      return new HiCommandHandler(null).handle().then(response => {
        assert.equal(response, 'Hi to you too!');
      });

    });
    it('should return response when command data is not null', () => {
      return new HiCommandHandler('something').handle().then(response => {
        assert.equal(response, 'Hi to you too!');
      });
    });
  });
});

