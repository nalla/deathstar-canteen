const chai = require('chai');

const CommandFactory = require('./CommandFactory');

describe('CommandFactory', () => {
  describe('#getCommand()', () => {
    it('should return null when the command name is not known', () => {
      const command = CommandFactory.getCommand('unknown', null);
      chai.assert.equal(command, null);
    });

    it('should return handler when command name is known', () => {
      const command = CommandFactory.getCommand('help', null);
      chai.assert.isOk(command);
    });
  });
});
