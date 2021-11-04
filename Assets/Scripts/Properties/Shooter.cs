using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject canon;
    public GameObject laserPrefab;
    public float speed = 10f;
    public float laserHitDuration = 0.1f;

    private float canonYOffset;
    private Block hitBlock;
    private Direction? hitDirection;
    private bool isShooting = false;
    private GameObject laser;
    private Direction shootDirection;
    private Vector2Int shootPosition;
    private List<Vector3> trajectoryList;

    public void AddLaserPoint(Vector2 position, float speed, Action callback)
    {
        LineRenderer line = laser.GetComponent<LineRenderer>();
        int pointsCount = line.positionCount + 1;
        line.positionCount = pointsCount;
        int i = pointsCount - 2;
        Vector3 startPosition = line.GetPosition(i);
        Vector3 endPosition = new Vector3(position.x, startPosition.y, position.y);
        StartCoroutine(AnimateLineSegment(line, i, pointsCount, startPosition, endPosition, speed, callback));
    }

    public void DestroyLaser()
    {
        Destroy(laser, laserHitDuration);
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public void SetIsShooting(bool isShooting)
    {
        this.isShooting = isShooting;
    }

    public void Shoot(Vector2Int position, Direction direction, Action callback = null)
    {
        isShooting = true;
        trajectoryList = new List<Vector3>();
        canonYOffset = canon.transform.position.y;
        trajectoryList.Add(canon.transform.position);

        shootDirection = direction;
        shootPosition = DirectionHelper.GetNextPosition(position, direction);

        StartCoroutine(AnimateLine(callback));
    }

    private IEnumerator AnimateLine(Action callback = null)
    {
        bool continueShooting = true;
        int i = 0;
        hitBlock = null;
        hitDirection = null;

        laser = Instantiate(laserPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        LineRenderer line = laser.GetComponent<LineRenderer>();
        laser.SetActive(true);

        while (continueShooting)
        {
            CheckNextPosition(shootPosition, ref shootDirection, ref continueShooting);
            int pointsCount = trajectoryList.Count;
            line.positionCount = pointsCount;

            if (pointsCount > 1 && i < pointsCount - 1) {
                Vector3 startPosition = trajectoryList[i];
                Vector3 endPosition = trajectoryList[i + 1];
                yield return StartCoroutine(AnimateLineSegment(line, i++, pointsCount, startPosition, endPosition, speed));
            }
            else
            {
                continueShooting = false;
            }
        }

        ShootEnd(() => StartCoroutine(ShootEndCallback(callback)));
    }

    private IEnumerator AnimateLineSegment(LineRenderer line, int i, int pointsCount, Vector3 startPosition, Vector3 endPosition, float speed, Action callback = null)
    {
        float startTime = Time.time;
        Vector3 pos = startPosition;
        float distance = Vector3.Distance(startPosition, endPosition);
        float segmentDuration = distance / speed;
        line.SetPosition(i, startPosition);

        while (pos != endPosition)
        {
            float t = (Time.time - startTime) / segmentDuration;
            pos = Vector3.Lerp(startPosition, endPosition, t);
            line.SetPosition(i + 1, pos);

            yield return null;
        }
        line.SetPosition(i + 1, endPosition);

        if (callback != null)
        {
            callback();
        }
    }

    private void CheckNextPosition(Vector2Int position, ref Direction direction, ref bool continueShooting)
    {
        Block objectBlock = null;
        Board board = BoardManager.current.GetBoard();

        if (board.IsPositionInRange(position))
        {
            continueShooting = true;
            GameObject objectObject = board.GetObjectBlock(position.x, position.y);

            if (objectObject != null)
            {
                objectBlock = objectObject.GetComponent<Block>();

                if (objectBlock != null)
                {
                    Vector3 trajectoryPosition = objectBlock.GetShootHitPosition(canonYOffset, ref direction, ref continueShooting);
                    trajectoryList.Add(trajectoryPosition);
                }
            }

            if (objectBlock == null)
            {
                Vector3 trajectoryPosition = DirectionHelper.GetShootHitPosition(position, direction, canonYOffset, BoardManager.current.tileSize);
                trajectoryList.Add(trajectoryPosition);
            }
        }
        else
        {
            continueShooting = false;
        }

        if (continueShooting)
        {
            shootPosition = DirectionHelper.GetNextPosition(position, direction);
        }
        else if (objectBlock != null)
        {
            hitBlock = objectBlock;
            hitDirection = direction;
        }
    }

    private void ShootEnd(Action callback)
    {
        if (hitBlock != null)
        {
            hitBlock.ShootThrough(gameObject, (Direction)hitDirection, this, callback);
        }
        else
        {
            DestroyLaser();
            callback();
        }
    }

    private IEnumerator ShootEndCallback(Action callback = null)
    {
        yield return new WaitForSeconds(laserHitDuration);
        isShooting = false;
        if (callback != null)
        {
            callback();
        }
    }
}
