package sfs2x.extensions.games.dice;

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
		
		ChipController.getInstance().moveChips(params.getInt("amount"), params.getInt("from"), params.getInt("to"));
		
		ISFSObject rtn = new SFSObject();
		rtn.putIntArray("chipList", ChipController.getInstance().getChipList());
		
		gameExt.send("recieveChipList", rtn, gameExt.getGameRoom().getUserList());
	}
}
