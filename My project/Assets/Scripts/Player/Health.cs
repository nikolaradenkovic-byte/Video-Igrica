using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public UIManager manager;
    public int health = 500;
    public int score = 0;
    public TMP_Text healthText;

    public void TakeDamage(int dmg)
    {
        AudioManager.Instance.PlaySFX("hurt");
        health -= dmg;

        healthText.SetText("HP: " + health);
        if (health <= 0)
            Die();
    }

    public void takeHp(int hp)
    {
        score += 150;
        manager.setScore(score);
        health += hp;

        if(health > 500)
        {
            health = 500;
        }
        healthText.SetText("HP: " + health);
    }
    void Die()
    {
        manager.setScore(score);
        manager.death();
    }
}
