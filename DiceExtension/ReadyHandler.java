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
			
			if (gameExt.getMaxPlayers() == null)
			{
				int numOfPlayers = params.getInt("num");
				gameExt.setMaxPlayers(numOfPlayers);
			}
			
			// Checks if all players are available and starts the game
			if (gameExt.canStartGame())
				gameExt.startGame();
		}
		
		else
		{
			trace("ready handler called: not a player");
		}
	}
}
