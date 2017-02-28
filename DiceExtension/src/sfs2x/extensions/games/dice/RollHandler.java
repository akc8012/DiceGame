package sfs2x.extensions.games.dice;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class RollHandler extends BaseClientRequestHandler
{
	@Override
	public void handleClientRequest(User player, ISFSObject params)
	{
		DiceExtension parentExt = (DiceExtension)getParentExtension();
		
		trace("player with id: " + player.getPlayerId() + " got to the roll thing");
		
		if (parentExt.getWhoseTurn() == player)
		{
			int roll = params.getInt("roll");
			int newTurn = parentExt.updateTurn();
			
			ISFSObject rtn = new SFSObject();
			rtn.putInt("roll", roll);
			rtn.putInt("turn", newTurn);
			
			trace("roll: " + roll +" turn: " + player.getPlayerId());
			
			parentExt.send("getRoll", rtn, parentExt.getGameRoom().getUserList());
		}
	}

}
