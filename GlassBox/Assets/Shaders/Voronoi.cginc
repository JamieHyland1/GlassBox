#ifndef VORONOI
#define VORONOI
    float2 voronoiNoise2d(float2 value){
            float2 baseCell = floor(value);
            float minDistToCell = 10;
            float2 closestCell;


            [unroll]
            for(int x = -1; x<=1;x++){
                [unroll]
                for(int y = -1; y <=1; y++){
                    float2 cell = baseCell + float2(x,y);
                    float2 cellPosition = cell + rand2dTo2d(cell);
                    float2 toCell = cellPosition-value;
                    float distToCell = length(toCell);
                    if(distToCell < minDistToCell){
                        minDistToCell = distToCell;
                        closestCell = cell;
                    }
                }
            }
            float random = rand2dTo1d(closestCell);
            return float2(minDistToCell,random);
        }

        float voronoiNoise1d(float2 value){
            float2 baseCell = floor(value);
            float minDistToCell = 10;
          


            [unroll]
            for(int x = -1; x<=1;x++){
                [unroll]
                for(int y = -1; y <=1; y++){
                    float2 cell = baseCell + float2(x,y);
                    float2 cellPosition = cell + rand2dTo2d(cell);
                    float2 toCell = cellPosition-value;
                    float distToCell = length(toCell);
                    if(distToCell < minDistToCell){
                        minDistToCell = distToCell;
                    }
                }
            }
            
            return minDistToCell;
        }

    #endif

