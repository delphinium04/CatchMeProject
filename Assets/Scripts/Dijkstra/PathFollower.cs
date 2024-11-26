using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Dijkstra
{
    public class PathFollower : MonoBehaviour
    {
        /// <summary>
        /// path의 Vector3를 속도와 거리에 비례해서 따라갑니다.
        /// 최종 목적지에 도달했을 경우 
        /// </summary>
        /// <param name="path">Tuple(position: Vector3, weight: int)[]</param>
        /// <param name="speed">속도(1초당 가는 거리)</param>
        /// <param name="onPathEnd">Callback function invoked when departed</param>
        public void FollowPath(Tuple<Vector3, int>[] path, float speed, Action onPathEnd, float delay = 0)
        {
            StartCoroutine(FollowPathCoroutine(path, speed, onPathEnd, delay));
        }

        private IEnumerator FollowPathCoroutine(Tuple<Vector3, int>[] path, float speed, Action onPathEnd,
            float delay = 0)
        {
            if(delay != 0) yield return new WaitForSeconds(delay);
            // 경로의 각 지점을 순서대로 이동
            foreach (var node in path)
            {
                Vector3 nextPosition = node.Item1;
                int weight = node.Item2;
                float travelDuration = weight / speed;

                // DOTween을 사용한 이동
                Tween moveTween = transform.DOMove(nextPosition, travelDuration).SetEase(Ease.Linear);

                // 이동이 끝날 때까지 대기
                yield return moveTween.WaitForCompletion();
            }

            // 모든 이동이 끝난 후 콜백 호출
            onPathEnd?.Invoke();
        }
    }
}