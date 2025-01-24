using System.Collections.Generic;
using System.Diagnostics;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    // Create a dictionary to store the values of each piece
    private Dictionary<PieceType, int> pieceValues = new Dictionary<PieceType, int>
    {
        {PieceType.Pawn, 1},
        {PieceType.Knight, 3},
        {PieceType.Bishop, 3},
        {PieceType.Rook, 5},
        {PieceType.Queen, 9},
        {PieceType.King, 0}
    };
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        Move bestMove = default;
        int maxEval = int.MinValue;
        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int eval = -Negamax(board, 3, int.MinValue, int.MaxValue);
            board.UndoMove(move);
            if (eval > maxEval)
            {
                maxEval = eval;
                bestMove = move;
            }
        }
        Debug.WriteLine($"Max eval: {maxEval}");
        Debug.WriteLine($"Best move: {bestMove}");
        return bestMove;
    }

    public int Negamax(Board board, int depth, int alpha, int beta)
    {
        if (depth == 0 || board.IsInCheckmate() || board.IsDraw())
        {
            return Evaluate(board);
        }

        Move[] moves = board.GetLegalMoves();
        int maxEval = int.MinValue;
        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int eval = -Negamax(board, depth - 1, -beta, -alpha);
            board.UndoMove(move);
            maxEval = System.Math.Max(maxEval, eval);
            alpha = System.Math.Max(alpha, eval);
            if (beta <= alpha)
            {
                break;
            }
        }
        return maxEval;
    }

    public int Evaluate(Board board)
    {
        int score = 0;
        bool isWhite = board.IsWhiteToMove;
        PieceList[] piecelists = board.GetAllPieceLists();

        foreach(PieceList pieceList in piecelists)
        {
            foreach(Piece piece in pieceList)
            {
                int value = pieceValues[piece.PieceType];
                if (piece.IsWhite == isWhite)
                {
                    score += value;
                }
                else
                {
                    score -= value;
                }
            }
        }
        return score;
    }
}