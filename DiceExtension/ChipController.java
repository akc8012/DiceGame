package sfs2x.extensions.games.dice;

import java.util.ArrayList;

public class ChipController
{
	private static ChipController instance = null;
	private int[] chips = { 40, 0, 0, 0, 0, 0 };

	protected ChipController()
	{
		// Exists only to defeat instantiation.
	}

	public static ChipController getInstance()
	{
		if (instance == null)
			instance = new ChipController();
		
		return instance;
	}
	
	public void initChips(int amountOfChips)
	{
		chips = new int[6];
		chips[0] = amountOfChips;
		chips[1] = 0;
		chips[2] = 0;
		chips[3] = 0;
		chips[4] = 0;
		chips[5] = 0;
	}
	
	public void moveChips(int amount, int from, int to)
	{
		chips[from] -= amount;
		if (chips[from] < 0)
		{
			// chips in the from box will be negative, so subtract this from the amount to add to "to" box
			amount += chips[from];
			chips[from] = 0;
		}
		
		chips[to] += amount;
	}
	
	public ArrayList<Integer> getChipList()
	{
		ArrayList<Integer> chipList = new ArrayList<Integer>();
		for (int i = 0; i < chips.length; i++)
			chipList.add(chips[i]);
		
		return chipList;
	}
	
	public String printChips()
	{
		String str = "";
		
		for (int i = 0; i < chips.length; i++)
		{
			str += chips[i];
			
			if (i < chips.length-1)
				str += ", ";
		}
		
		return str;
	}
}












