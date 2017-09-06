const chai = require('chai');

const CommandFactory = require('./CommandFactory');

describe('CommandFactory', () => {
  describe('#getCommand()', () => {
    it('should return null when the command name is not known', () => {
      // Act
      const command = CommandFactory.getCommand('unknown', null);

      // Assert
      chai.assert.equal(command, null);
    });

    it('should return handler when command name is known', () => {
      // Act
      const command = CommandFactory.getCommand('help', null);

      // Assert
      chai.assert.isOk(command);
    });
  });
});
