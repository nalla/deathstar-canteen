var SlackBot = require('slackbots');
var config = require('./config');
var bot = new SlackBot({
    token: config.token,
    name: config.name
});
var id = null;

bot.on('start', function(data) {
  bot.getUser(bot.name).then(user => {
    id = user.id;
  });
});

bot.on('message', function(data) {
  var params = {
    icon_emoji: config.emoji
  };
  var regex = new RegExp('\\<\\@' + id + '\\>\\s(\\w+)\\s?(.*)');
  if(data.channel && data.text)
  {
    var match = data.text.match(regex);
    if(match)
    {
      var commandName = match[1].toLowerCase();
      var commandData = match[2];
      switch(commandName)
      {
        case 'hi':
          bot.postMessage(data.channel, 'Hi to you too!', params);
          break;
        
        case 'help':
          if(!commandData)
            bot.postMessage(data.channel, 'The following commands are available:\n*hi*\n*help*', params);
          else
            bot.postMessage(data.channel, commandData, params);
         break;

        default:
          bot.postMessage(data.channel, 'I don\'t understand a word you saying.', params);
          break;
      }
    }
  }
});

