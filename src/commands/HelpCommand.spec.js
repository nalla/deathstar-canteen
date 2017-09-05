const chai = require('chai');

const HelpCommand = require('./HelpCommand');

describe('HelpCommand', () => {
  describe('#handle()', () => {
    it('should return general help message when the command data is null', async () => {
      const response = await new HelpCommand(null).handle();
      chai.assert.include(response, 'The following commands are available');
    });

    it('should return general help message when command data is not known', async () => {
      const response = await new HelpCommand('something').handle();
      chai.assert.include(response, 'The following commands are available');
    });

    it('should return detailed help message when command data is known', async () => {
      const response = await new HelpCommand('hi').handle();
      chai.assert.include(response, 'The *hi* command will return a friendly hello.');
    });
  });
});
