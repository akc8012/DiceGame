package sfs2x.extensions.games.dice;

//import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class DiceExtension extends SFSExtension
{
	private User whoseTurn;
	private Integer maxPlayers = null;
	private int additionalPlayers = 0;
	private volatile boolean gameStarted;
	
	@Override
	public void init()
	{	
		this.addRequestHandler("ready", ReadyHandler.class);
		this.addRequestHandler("sendRoll", RollHandler.class);
		this.addRequestHandler("sendChipMove", ChipHandler.class);
	}
	
	Integer getMaxPlayers()
	{
		return maxPlayers;
	}
	
	void setMaxPlayers(int players)
	{
		maxPlayers = players;
	}
	
	void setAdditionalPlayer() { additionalPlayers++; }
	
	boolean canStartGame()
	{
		return this.getGameRoom().getSize().getUserCount() + additionalPlayers == maxPlayers;
	}
	
	Room getGameRoom()
	{
		return this.getParentRoom();
	}
	
	int updateTurn()
	{
		int newTurn = whoseTurn.getPlayerId() - 1;
		
		if (newTurn < 1)
			newTurn = maxPlayers;
		
		trace("update turn: " + newTurn);
		
		whoseTurn = getParentRoom().getUserByPlayerId(newTurn);
		return newTurn;
	}
	
	User getWhoseTurn()
    {
	    return whoseTurn;
    }
	
	void startGame()
	{
		trace("extension start function called");
		
		if (gameStarted)
			throw new IllegalStateException("Game is already started!");
		
		gameStarted = true;
		
		User lastPlayer = getParentRoom().getUserByPlayerId(maxPlayers - additionalPlayers);
		
		// No turn assigned? Start with the last player
		if (whoseTurn == null)
			whoseTurn = lastPlayer;
		
		trace("whose turn: " + whoseTurn.getPlayerId());
		
		// Send START event to client
		ISFSObject resObj = new SFSObject();
		resObj.putInt("t", whoseTurn.getPlayerId());
		
		send("start", resObj, getGameRoom().getUserList());
	}

}





