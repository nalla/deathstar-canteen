'use strict';

const assert = require('chai').assert;

const HiCommand = require('../lib/commands/hi-command');

describe('HiCommand', () => {

  describe('#handle()', () => {

    it('should return response when the command data is null', () => {
      return new HiCommand(null).handle().then(response => {
        assert.equal(response, 'Hi to you too!');
      });
    });

    it('should return response when command data is not null', () => {
      return new HiCommand('something').handle().then(response => {
        assert.equal(response, 'Hi to you too!');
      });
    });
  });
});

