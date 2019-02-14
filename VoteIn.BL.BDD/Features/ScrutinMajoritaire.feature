Feature: ScrutinMajoritaire
    In order to avoid silly mistakes
    As a math idiot
    I want to be told the sum of two numbers

Background: 
    Given Un nouveau scrutin majoritaire "scrutin 1"

Scenario: Scrutin majoritaire un électeur et un vainqueur
    Given les options suivantes pour le scrutin "scrutin 1"
    | Nom        |
    | candidat 1 |
    | candidat 2 |
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    When je clôture le scrutin majoritaire "scrutin 1"
    Then "candidat 1" est désigné comme vainqueur
    And le résultat est valide
    And j'obtiens le résultat suivant
    | Option     | Nombre de vote | pourcentage |
    | candidat 1 | 1              | 100         |
    | candidat 2 | 0              | 0           |

Scenario: Scrutin majoritaire sans vainqueur avec second tour
    Given les options suivantes pour le scrutin "scrutin 1"
    | Nom        |
    | candidat 1 |
    | candidat 2 |
    | candidat 3 |
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 2" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 2" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 3" pour le scrutin majoritaire "scrutin 1"
    When je clôture le scrutin majoritaire "scrutin 1"
    Then il n'y a pas de vainqueur
    And le résultat est valide
    And j'obtiens le résultat suivant
    | Option     | Nombre de vote | pourcentage |
    | candidat 1 | 2              | 40          |
    | candidat 2 | 2              | 40          |
    | candidat 3 | 1              | 20          |

Scenario: Scrutin majoritaire sans vainqueur et sans second tour
    Given les options suivantes pour le scrutin "scrutin 1"
    | Nom        |
    | candidat 1 |
    | candidat 2 |
    | candidat 3 |
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 2" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 3" pour le scrutin majoritaire "scrutin 1"
    When je clôture le scrutin majoritaire "scrutin 1"
    Then il n'y a pas de vainqueur
    And le résultat n'est pas valide
    And j'obtiens le résultat suivant
    | Option     | Nombre de vote | pourcentage |
    | candidat 1 | 2              | 50          |
    | candidat 2 | 1              | 25          |
    | candidat 3 | 1              | 25          |


Scenario: Scrutin majoritaire ou le vote blanc gagne
    Given les options suivantes pour le scrutin "scrutin 1"
    | Nom        |
    | candidat 1 |
    And un electeur vote blanc pour le scrutin majoritaire "scrutin 1"
    When je clôture le scrutin majoritaire "scrutin 1"
    Then il n'y a pas de vainqueur
    And le résultat n'est pas valide
    And j'obtiens le résultat suivant
    | Option     | Nombre de vote | pourcentage |
    | Vote blanc | 1              | 100         |
    | candidat 1 | 0              | 0           |

Scenario: Scrutin majoritaire second tour moins de 50% sans egalite
    Given les options suivantes pour le scrutin "scrutin 1"
    | Nom        |
    | candidat 1 |
    | candidat 2 |
    | candidat 3 |
    And on est au second tour du scrutin "scrutin 1"
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 2" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 3" pour le scrutin majoritaire "scrutin 1"
    When je clôture le scrutin majoritaire "scrutin 1"
    Then "candidat 1" est désigné comme vainqueur
    And le résultat est valide

Scenario: Scrutin majoritaire second tour avec egalite
    Given les options suivantes pour le scrutin "scrutin 1"
    | Nom        |
    | candidat 1 |
    | candidat 2 |
    And on est au second tour du scrutin "scrutin 1"
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 1" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 2" pour le scrutin majoritaire "scrutin 1"
    And un electeur vote "candidat 2" pour le scrutin majoritaire "scrutin 1"
    When je clôture le scrutin majoritaire "scrutin 1"
    Then il n'y a pas de vainqueur
    And le résultat n'est pas valide