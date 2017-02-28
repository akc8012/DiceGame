package sfs2x.extensions.games.dice;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ReadyHandler extends BaseClientRequestHandler
{
	@Override
	public void handleClientRequest(User user, ISFSObject params)
	{
		DiceExtension gameExt = (DiceExtension)getParentExtension();
		
		if (user.isPlayer())
		{
			trace("ready handler called");
			// Checks if five players are available and start game
			if (gameExt.getGameRoom().getSize().getUserCount() == 5)
				gameExt.startGame();
		}
		
		else
		{
			trace("ready handler called: not a player");
		}
	}
}
