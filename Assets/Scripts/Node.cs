using UnityEngine;

namespace BombermanGame
{
    public class Node
    {
        public Vector2 Position;
        public Vector2 TargetPosition;
        public Node PreviousNode;
        public int CostOfMovement; // G+H
        public int DistanceFromStartToNode; //from start to Node
        public int DistanceFromNodeToTarget; //from Node to Target

        public Node(int distanceFromStartToNode, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
        {
            Position = nodePosition;
            TargetPosition = targetPosition;
            PreviousNode = previousNode;
            DistanceFromStartToNode = distanceFromStartToNode;
            DistanceFromNodeToTarget = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
            CostOfMovement = DistanceFromStartToNode + DistanceFromNodeToTarget;
        }
    }
}