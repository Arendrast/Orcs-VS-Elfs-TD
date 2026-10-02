# Tower Defense Playable

The project is a **Tower Defense playable** made with **Unity 6000.0.70f1** for *Azur Games* using the provided assets (sounds, models, images). On top of that, I added an arrow effect and unit upgrades of my own to make it look nice

---

### Demo
https://github.com/user-attachments/assets/3026974f-344a-4920-90fb-85ddee6ee0c3

https://github.com/user-attachments/assets/11181a50-ae4d-44e0-8849-525531bfcb64

https://github.com/user-attachments/assets/d33026c9-2c85-4b96-bd3a-e46b8aa6d87d

---

### Implementation details
The requirements were maximum performance, easy extensibility and a component-based approach. So I followed a few principles:

1. **Minimized** the number of `GetComponent` calls
2. **Avoided Instantiate** wherever possible, and where I did use it, there is a custom pool
3. **Optimized the build size**: I didn't add any libraries except *DoTween*, to keep the size minimal and avoid the pitfalls of third-party libraries. The project is also built completely from scratch, so it is fully under control with a minimum of unused code!
4. Architecture: I used an *MVC + EC (entity component)* combo so tests are easy to write (see the merge grid tests in `Assets\Modules\PlayerUnitModule\Scripts\Tests`)
5. *Dropped physics entirely*: all mobs are driven by lightweight *DOTween* tweens, and the only `Raycast` in the game detects which archer cell the player tapped
6. Switched the project to the *URP* pipeline and enabled the *SRP Batcher*. Long live optimization and a modern pipeline!
7. Split the mechanics into modules, each with its own assembly
8. Used a service and factory approach to keep things manageable (who spawns objects, where data comes from)
9. Implemented game states but skipped a state machine, since the game is small. The game is initialized through manual DI and started by the `Bootstrapper` script
10. Wrote the modules with the future in mind: abstractions like `Entity` and `Unit`, and shared components like `HealthComponent` and `DamageableComponent`. For example, the attack system lets characters use different attack types once they are added

---

*I tried to make it look good, happy to hear any feedback! :)*
