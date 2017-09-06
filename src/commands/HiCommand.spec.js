const chai = require('chai');

const HiCommand = require('./HiCommand');

describe('HiCommand', () => {
  describe('#handle()', () => {
    it('should return response when the command data is null', async () => {
      // Act
      const response = await new HiCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'Hi to you too!');
    });

    it('should return response when command data is not null', async () => {
      // Act
      const response = await new HiCommand('something').handle();

      // Assert
      chai.assert.equal(response, 'Hi to you too!');
    });
  });
});
