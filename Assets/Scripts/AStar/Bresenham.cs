﻿

  ///***************************************************************************/

  //void SmoothPath( List<Node> path )
  //{
  //   TODO
  //}

  ///***************************************************************************/

  //public bool BresenhamWalkable(int x, int y, int x2, int y2)
  //{
  //   TODO: 4 Connectivity
  //   TODO: Cost

  //  int w = x2 - x;
  //  int h = y2 - y;
  //  int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
  //  if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
  //  if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
  //  if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
  //  int longest = Mathf.Abs(w);
  //  int shortest = Mathf.Abs(h);
  //  if (!(longest > shortest)){
  //    longest = Mathf.Abs(h);
  //    shortest = Mathf.Abs(w);
  //    if (h < 0){
  //      dy2 = -1;
  //    }
  //    else if (h > 0){
  //      dy2 = 1;
  //    }
  //    dx2 = 0;
  //  }
  //  int numerator = longest >> 1;
  //  for (int i = 0; i <= longest; i++)
  //  {
  //    if( !Grid.GetNode(x, y).mWalkable){
  //      return false;
  //    }
      
  //    numerator += shortest;
  //    if (!(numerator < longest)){
  //      numerator -= longest;
  //      x += dx1;
  //      y += dy1;
  //    }
  //    else{
  //      x += dx2;
  //      y += dy2;
  //    }
  //  }

  //  return true;
  //}

 