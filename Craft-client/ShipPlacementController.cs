using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Craft_client
{
    /// <summary>
    /// Disables buttons for player panel
    /// </summary>
    public class ShipPlacementController
    {
        // Ship count for {cruiser, submarine, warship, aircarrier} by windows they occupy
        public int[] Ship_Count = new int[4] { 4, 6, 6, 4 };
        // Log for last ships placed
        public int[] Last_Row = new int[100]; 
        public int[] Last_Collumn = new int[100]; 
        public int Last_step_count = 0;

        /// <summary>
        /// Disable buttons around Aircarrier for player - ships can not touch
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        public Button[,] Aircarrier_Placed(int row, int collumn, Button[,] button_grid_player)
        {
            if (Ship_Count[3] != 0) // if it is not the last tile placed for Aircarrier type ship
            {
                button_grid_player = Limit_Placement_Directions(row, collumn, button_grid_player);
            }
            else //last tile was placed 4/4
            {
                if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
                if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
                if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
                if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }

                Last_step_count = Last_step_count - 1;
                if (row != 9 && row + 1 != Last_Row[Last_step_count]) { button_grid_player[row + 1, collumn].Enabled = false; }
                if (row != 0 && row - 1 != Last_Row[Last_step_count]) { button_grid_player[row - 1, collumn].Enabled = false; }
                if (collumn != 9 && collumn + 1 != Last_Collumn[Last_step_count]) { button_grid_player[row, collumn + 1].Enabled = false; }
                if (collumn != 0 && collumn - 1 != Last_Collumn[Last_step_count]) { button_grid_player[row, collumn - 1].Enabled = false; }

                Last_step_count = Last_step_count - 2;
                if (Last_Row[Last_step_count] != 9 && Last_Row[Last_step_count] + 1 != Last_Row[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count] + 1, Last_Collumn[Last_step_count]].Enabled = false; }
                if (Last_Row[Last_step_count] != 0 && Last_Row[Last_step_count] - 1 != Last_Row[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count] - 1, Last_Collumn[Last_step_count]].Enabled = false; }
                if (Last_Collumn[Last_step_count] != 9 && Last_Collumn[Last_step_count] + 1 != Last_Collumn[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count], Last_Collumn[Last_step_count] + 1].Enabled = false; }
                if (Last_Collumn[Last_step_count] != 0 && Last_Collumn[Last_step_count] - 1 != Last_Collumn[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count], Last_Collumn[Last_step_count] - 1].Enabled = false; }
                Last_step_count = Last_step_count + 3;
            }

            return button_grid_player;
        }

        /// <summary>
        /// Disable buttons around Warship for player - ships can not touch
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        public Button[,] Warship_Placed(int row, int collumn, Button[,] button_grid_player)
        {
            if (Ship_Count[2] != 3 && Ship_Count[2] != 0) // if tile placed was not the last of a single Warship type ship
            {
                button_grid_player = Limit_Placement_Directions(row, collumn, button_grid_player);
            } else //last tile was placed 3/3
            {
                if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
                if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
                if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
                if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }

                Last_step_count = Last_step_count - 1;
                if (row != 9 && row + 1 != Last_Row[Last_step_count]) { button_grid_player[row + 1, collumn].Enabled = false; }
                if (row != 0 && row - 1 != Last_Row[Last_step_count]) { button_grid_player[row - 1, collumn].Enabled = false; }
                if (collumn != 9 && collumn + 1 != Last_Collumn[Last_step_count]) { button_grid_player[row, collumn + 1].Enabled = false; }
                if (collumn != 0 && collumn - 1 != Last_Collumn[Last_step_count]) { button_grid_player[row, collumn - 1].Enabled = false; }

                Last_step_count = Last_step_count - 1;
                if (Last_Row[Last_step_count] != 9 && Last_Row[Last_step_count] + 1 != Last_Row[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count] + 1, Last_Collumn[Last_step_count]].Enabled = false; }
                if (Last_Row[Last_step_count] != 0 && Last_Row[Last_step_count] - 1 != Last_Row[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count] - 1, Last_Collumn[Last_step_count]].Enabled = false; }
                if (Last_Collumn[Last_step_count] != 9 && Last_Collumn[Last_step_count] + 1 != Last_Collumn[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count], Last_Collumn[Last_step_count] + 1].Enabled = false; }
                if (Last_Collumn[Last_step_count] != 0 && Last_Collumn[Last_step_count] - 1 != Last_Collumn[Last_step_count + 1]) { button_grid_player[Last_Row[Last_step_count], Last_Collumn[Last_step_count] - 1].Enabled = false; }
                Last_step_count = Last_step_count + 2;
            }
            return button_grid_player;
        }

        /// <summary>
        /// Disable buttons around Submarine for player - ships can not touch
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        public Button[,] Submarine_Placed(int row, int collumn, Button[,] button_grid_player)
        {
            if (Ship_Count[1] % 2 != 0) // if ship placement was NOT started (1/2)
            {
                button_grid_player = Limit_Placement_Directions(row, collumn, button_grid_player);
            }
            else //ship placement was started - end the placement (2/2)
            {
                if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
                if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
                if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
                if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }

                Last_step_count = Last_step_count - 1;
                if (row != 9 && row + 1 != Last_Row[Last_step_count]) { button_grid_player[row + 1, collumn].Enabled = false; }
                if (row != 0 && row - 1 != Last_Row[Last_step_count]) { button_grid_player[row - 1, collumn].Enabled = false; }
                if (collumn != 9 && collumn + 1 != Last_Collumn[Last_step_count]) { button_grid_player[row, collumn + 1].Enabled = false; }
                if (collumn != 0 && collumn - 1 != Last_Collumn[Last_step_count]) { button_grid_player[row, collumn - 1].Enabled = false; }

                if (Last_Row[Last_step_count] != 9 && Last_Row[Last_step_count] + 1 != row) { button_grid_player[Last_Row[Last_step_count] + 1, Last_Collumn[Last_step_count]].Enabled = false; }
                if (Last_Row[Last_step_count] != 0 && Last_Row[Last_step_count] - 1 != row) { button_grid_player[Last_Row[Last_step_count] - 1, Last_Collumn[Last_step_count]].Enabled = false; }
                if (Last_Collumn[Last_step_count] != 9 && Last_Collumn[Last_step_count] + 1 != collumn) { button_grid_player[Last_Row[Last_step_count], Last_Collumn[Last_step_count] + 1].Enabled = false; }
                if (Last_Collumn[Last_step_count] != 0 && Last_Collumn[Last_step_count] - 1 != collumn) { button_grid_player[Last_Row[Last_step_count], Last_Collumn[Last_step_count] - 1].Enabled = false; }
                Last_step_count = Last_step_count + 2;
            }
            return button_grid_player;
        }

        /// <summary>
        /// Disable buttons around cruiser for player board - ships can not touch
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        public Button[,] Cruiser_Placed(int row, int collumn, Button[,] button_grid_player)
        {
            if (row != 9) { button_grid_player[row + 1, collumn].Enabled = false; }
            if (row != 0) { button_grid_player[row - 1, collumn].Enabled = false; }
            if (collumn != 9) { button_grid_player[row, collumn + 1].Enabled = false; }
            if (collumn != 0) { button_grid_player[row, collumn - 1].Enabled = false; }

            if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
            if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
            if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
            if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }

            return button_grid_player;
        }

        private Button[,] Limit_Placement_Directions(int row, int collumn, Button[,] button_grid_player)
        {
            if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
            if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
            if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
            if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }

            Last_Row[Last_step_count] = row; Last_Collumn[Last_step_count] = collumn;

            return button_grid_player;
        }
    }
}
