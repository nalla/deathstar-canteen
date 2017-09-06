const chai = require('chai');
const moment = require('moment');

const TomorrowCommand = require('./TomorrowCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('TomorrowCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should return tomorrow\'s menu', async () => {
      await Menu.create({ date: moment().add(1, 'days').format('YYYYMMDD'), meals: ['Foo', 'Bar'] });
      const expected = `Tomorrow is the *${moment().add(1, 'days').format('DD.MM.YYYY')}* and the meals are:\n1. Foo\n2. Bar\n`;
      const response = await new TomorrowCommand(null).handle();
      chai.assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      const response = await new TomorrowCommand(null).handle();
      chai.assert.equal(response, 'I don\'t know which meals are being served tomorrow!');
    });
  });
});
