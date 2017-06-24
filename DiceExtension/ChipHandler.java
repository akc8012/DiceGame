package sfs2x.extensions.games.dice;

import java.util.Collection;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ChipHandler extends BaseClientRequestHandler
{
	@Override
	public void handleClientRequest(User user, ISFSObject params)
	{
		DiceExtension gameExt = (DiceExtension)getParentExtension();
		
		Collection<Integer> chipData = params.getIntArray("data");
		
		ISFSObject rtn = new SFSObject();
		rtn.putIntArray("data", chipData);
		
		trace("sending chip data");
		
		gameExt.send("getChipData", rtn, gameExt.getGameRoom().getUserList());
	}
}
