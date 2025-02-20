using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAnimator : MonoBehaviour
{
    public NavMeshAgent agent; // Агент
    public Animator animator; // Аниматор

    private bool canMove = true;

    private void Start()
    {
        agent = agent.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Проверка на достижение цели
        if (agent.remainingDistance <= agent.stoppingDistance && this.GetComponent<NPCAI>().pickUpStage != 5)
        {
            StopMovement(16);
        }
    }

    // Универсальная функция для перемещения к объекту
    public void MoveToObject(GameObject targetObject, bool run = false)
    {
        // Анимация движения
        animator.SetInteger("arms", 1);
        animator.SetInteger("legs", 1);
    }

    private void StopMovement(int hand) // 4 16
    {
        animator.SetInteger("arms", 16);
        animator.SetInteger("legs", 0);
    }
}
