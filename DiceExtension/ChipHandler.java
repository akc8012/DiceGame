package sfs2x.extensions.games.dice;

import java.util.ArrayList;
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
		
		moveChips(gameExt, params.getInt("amount"), params.getInt("from"), params.getInt("to"));
		
		ISFSObject rtn = new SFSObject();
		rtn.putIntArray("chipList", getChipList(gameExt));
		
		gameExt.send("recieveChipList", rtn, gameExt.getGameRoom().getUserList());
	}
	
	/*public void initChips(int amountOfChips)
	{
		chips = new int[5];
		chips[0] = amountOfChips;
		chips[1] = 0;
		chips[2] = 0;
		chips[3] = 0;
		chips[4] = 0;
	}*/
	
	private void moveChips(DiceExtension gameExt, int amount, int from, int to)
	{
		gameExt.chips[from] -= amount;
		if (gameExt.chips[from] < 0)
		{
			// chips in the from box will be negative, so subtract this from the amount to add to "to" box
			amount += gameExt.chips[from];
			gameExt.chips[from] = 0;
		}
		
		gameExt.chips[to] += amount;
		
		printChips(gameExt);
	}
	
	private ArrayList<Integer> getChipList(DiceExtension gameExt)
	{
		ArrayList<Integer> chipList = new ArrayList<Integer>();
		for (int i = 0; i < gameExt.chips.length; i++)
			chipList.add(gameExt.chips[i]);
		
		return chipList;
	}
	
	private void printChips(DiceExtension gameExt)
	{
		String str = "";
		
		for (int i = 0; i < gameExt.chips.length; i++)
		{
			str += gameExt.chips[i];
			
			if (i < gameExt.chips.length-1)
				str += ", ";
		}
		
		trace(str);
	}
}









