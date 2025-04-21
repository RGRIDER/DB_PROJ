--SBSE PEHLE YE TABLES CREAT HONE
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(15),
    role VARCHAR(15) NOT NULL CHECK (role IN ('admin', 'member', 'trainer', 'gym_owner')),
    registration_date DATE DEFAULT GETDATE(),
    status VARCHAR(15) DEFAULT 'active' CHECK (status IN ('active', 'inactive', 'banned'))
);

CREATE TABLE Machines (
    machine_id INT PRIMARY KEY IDENTITY(1,1),
    machine_name VARCHAR(100) NOT NULL,
    type VARCHAR(50) NOT NULL,
    status VARCHAR(15) DEFAULT 'available' CHECK (status IN ('available', 'maintenance', 'unavailable'))
);




--DOOSRE NUMBER PR YE
CREATE TABLE Gyms (
    gym_id INT PRIMARY KEY IDENTITY(1,1),
    owner_id INT NOT NULL,
    gym_name VARCHAR(100) NOT NULL,
    address VARCHAR(255) NOT NULL,
    registration_date DATE DEFAULT GETDATE(),
    status VARCHAR(15) DEFAULT 'pending' CHECK (status IN ('pending', 'active', 'inactive')),
    FOREIGN KEY (owner_id) REFERENCES Users(user_id)
);



CREATE TABLE Trainers (
    trainer_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT NOT NULL,
    gym_id INT NOT NULL,
    experience INT,
    specialty VARCHAR(50),
    rating FLOAT DEFAULT 0.0,
    rating_count INT DEFAULT 0,
    status VARCHAR(15) DEFAULT 'active' CHECK (status IN ('active', 'inactive', 'banned')),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (gym_id) REFERENCES Gyms(gym_id)
);


CREATE TABLE Exercises (
    exercise_id INT PRIMARY KEY IDENTITY(1,1),
    exercise_name VARCHAR(100) NOT NULL,
    description VARCHAR(255),
    target_muscle VARCHAR(50) NOT NULL,
    machine_id INT,
    FOREIGN KEY (machine_id) REFERENCES Machines(machine_id)
);




CREATE TABLE WorkoutPlans (
    plan_id INT PRIMARY KEY IDENTITY(1,1),
    creator_id INT NOT NULL,
    title VARCHAR(100) NOT NULL UNIQUE,
    goal VARCHAR(50) NOT NULL,
    plan_date DATE DEFAULT GETDATE(),
    status VARCHAR(15) DEFAULT 'active' CHECK (status IN ('active', 'inactive')),
    FOREIGN KEY (creator_id) REFERENCES Users(user_id)
);


CREATE TABLE DietPlans (
    diet_id INT PRIMARY KEY IDENTITY(1,1),
    creator_id INT NOT NULL,
    title VARCHAR(100) NOT NULL,
    type VARCHAR(50) CHECK (type IN ('omnivore', 'vegan', 'vegetarian', 'pescatarian', 'gluten-free', 'lactose-free')),
    goal VARCHAR(50) NOT NULL,
    plan_date DATE DEFAULT GETDATE(),
    status VARCHAR(15) DEFAULT 'active' CHECK (status IN ('active', 'inactive')),
    FOREIGN KEY (creator_id) REFERENCES Users(user_id)
);




--TEESRE NUMBER PR YE
CREATE TABLE DietPlanUsage (
    usage_id INT PRIMARY KEY IDENTITY(1,1),
    member_id INT NOT NULL,
    diet_id INT NOT NULL,
    creator_id INT NOT NULL,
    FOREIGN KEY (member_id) REFERENCES Users(user_id),
    FOREIGN KEY (diet_id) REFERENCES DietPlans(diet_id),
    FOREIGN KEY (creator_id) REFERENCES Users(user_id)
);




CREATE TABLE MemberPlanUsage (
    usage_id INT PRIMARY KEY IDENTITY(1,1),
    member_id INT NOT NULL,
    plan_id INT NOT NULL,
    creator_id INT NOT NULL,
    FOREIGN KEY (member_id) REFERENCES Users(user_id),
    FOREIGN KEY (plan_id) REFERENCES WorkoutPlans(plan_id),
    FOREIGN KEY (creator_id) REFERENCES Users(user_id)
);




CREATE TABLE Memberships (
    membership_id INT PRIMARY KEY IDENTITY(1,1), 
    member_id INT,
	membership_cost FLOAT NOT NULL CHECK (membership_cost >= 0),
    gym_id INT,
	weightt Float,
	Height Float,
    membership_type VARCHAR(10) CHECK (membership_type IN ('monthly', 'yearly')) NOT NULL, 
    start_date DATE NOT NULL,
    end_date DATE,
    status VARCHAR(10) CHECK (status IN ('active', 'expired', 'cancelled')) DEFAULT 'active', 
    FOREIGN KEY (member_id) REFERENCES Users(user_id),
    FOREIGN KEY (gym_id) REFERENCES Gyms(gym_id)
);




CREATE TABLE WorkoutExercises (
    workout_plan_id INT NOT NULL,
    exercise_id INT NOT NULL,
    sets INT NOT NULL,
    reps INT NOT NULL,
    rest_interval INT,
    PRIMARY KEY (workout_plan_id, exercise_id),
    FOREIGN KEY (workout_plan_id) REFERENCES WorkoutPlans(plan_id),
    FOREIGN KEY (exercise_id) REFERENCES Exercises(exercise_id)
);



CREATE TABLE Meals (
    meal_id INT PRIMARY KEY IDENTITY(1,1),
    meal_name VARCHAR(100) NOT NULL,
    calories INT NOT NULL,
    protein FLOAT,
    carbs FLOAT,
    fat FLOAT,
    fiber FLOAT,
    allergens VARCHAR(255)
);

CREATE TABLE Appointments (
    appointment_id INT PRIMARY KEY IDENTITY(1,1),
    trainer_id INT NOT NULL,
    member_id INT NOT NULL,
    appointment_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    status VARCHAR(15) DEFAULT 'pending' CHECK (status IN ('pending', 'approved', 'rejected')),
    FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id),
    FOREIGN KEY (member_id) REFERENCES Users(user_id)
);



CREATE TABLE TrainerFeedback (
    feedback_id INT PRIMARY KEY IDENTITY(1,1),
    member_id INT NOT NULL,
    trainer_id INT NOT NULL,
    rating FLOAT NOT NULL,
    comment VARCHAR(255),
    feedback_date DATE DEFAULT GETDATE(),
    FOREIGN KEY (member_id) REFERENCES Users(user_id),
    FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id)
);




CREATE TABLE DietPlanMeals (
    diet_id INT NOT NULL,
    meal_id INT NOT NULL,
    PRIMARY KEY (diet_id, meal_id),
    FOREIGN KEY (diet_id) REFERENCES DietPlans(diet_id),
    FOREIGN KEY (meal_id) REFERENCES Meals(meal_id)
);





































-- Trigger to enforce constraints and adjust default values on the Users table upon insertion or updating
CREATE TRIGGER trg_Enforce_User_Constraints
ON Users
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check username uniqueness
    IF EXISTS (
        SELECT username
        FROM inserted
        GROUP BY username
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR ('Username must be unique.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check email uniqueness
    IF EXISTS (
        SELECT email
        FROM inserted
        GROUP BY email
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR ('Email must be unique.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check role constraint
    IF EXISTS (
        SELECT role
        FROM inserted
        WHERE role NOT IN ('admin', 'member', 'trainer', 'gym_owner')
    )
    BEGIN
        RAISERROR ('Invalid role! Role must be one of: admin, member, trainer, gym_owner.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('active', 'inactive', 'banned')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: active, inactive, banned.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_user_id INT;
    SELECT @max_user_id = ISNULL(MAX(user_id), 0) FROM Users;
    SET @max_user_id = @max_user_id + 1;

    -- Set default values for missing or incorrect data
    UPDATE u
    SET 
        registration_date = COALESCE(u.registration_date, GETDATE()),
        status = COALESCE(u.status, 'active')
    FROM Users u
    JOIN inserted i ON u.user_id = i.user_id;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum user_id plus 1
        DBCC CHECKIDENT ('Users', RESEED, @max_user_id);
    END;
END;
GO





-- Trigger to enforce constraints on the Machines table upon insertion or updating
CREATE TRIGGER trg_Enforce_Machine_Constraints
ON Machines
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check machine_name uniqueness
    IF EXISTS (
        SELECT machine_name
        FROM inserted
        GROUP BY machine_name
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR ('Machine name must be unique.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('available', 'maintenance', 'unavailable')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: available, maintenance, unavailable.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_machine_id INT;
    SELECT @max_machine_id = ISNULL(MAX(machine_id), 0) FROM Machines;
    SET @max_machine_id = @max_machine_id + 1;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum machine_id plus 1
        DBCC CHECKIDENT ('Machines', RESEED, @max_machine_id);
    END;
END;
GO



-- Trigger to enforce constraints on the Gyms table upon insertion or updating
CREATE TRIGGER trg_Enforce_Gym_Constraints
ON Gyms
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('pending', 'active', 'inactive')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: pending, active, inactive.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if owner_id exists in Users table
    IF EXISTS (
        SELECT owner_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.owner_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Owner ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_gym_id INT;
    SELECT @max_gym_id = ISNULL(MAX(gym_id), 0) FROM Gyms;
    SET @max_gym_id = @max_gym_id + 1;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum gym_id plus 1
        DBCC CHECKIDENT ('Gyms', RESEED, @max_gym_id);
    END;
END;
GO



-- Trigger to enforce constraints and adjust default values on the Trainers table upon insertion or updating
CREATE TRIGGER trg_Enforce_Trainer_Constraints
ON Trainers
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('active', 'inactive', 'banned')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: active, inactive, banned.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if user_id exists in Users table
    IF EXISTS (
        SELECT u.user_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.user_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('User ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if gym_id exists in Gyms table
    IF EXISTS (
        SELECT g.gym_id
        FROM inserted AS i
        LEFT JOIN Gyms AS g ON i.gym_id = g.gym_id
        WHERE g.gym_id IS NULL
    )
    BEGIN
        RAISERROR ('Gym ID does not exist in Gyms table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_trainer_id INT;
    SELECT @max_trainer_id = ISNULL(MAX(trainer_id), 0) FROM Trainers;
    SET @max_trainer_id = @max_trainer_id + 1;

    -- Set default values for missing or incorrect data
    UPDATE t
    SET 
        rating = COALESCE(t.rating, 0.0),
        rating_count = COALESCE(t.rating_count, 0),
        status = COALESCE(t.status, 'inactive')
    FROM Trainers t
    JOIN inserted i ON t.trainer_id = i.trainer_id;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum trainer_id plus 1
        DBCC CHECKIDENT ('Trainers', RESEED, @max_trainer_id);
    END;
END;
GO


-- Trigger to enforce constraints on the Exercises table upon insertion or updating
CREATE TRIGGER trg_Enforce_Exercise_Constraints
ON Exercises
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if machine_id exists in Machines table
    IF EXISTS (
        SELECT m.machine_id
        FROM inserted AS i
        LEFT JOIN Machines AS m ON i.machine_id = m.machine_id
        WHERE m.machine_id IS NULL AND i.machine_id IS NOT NULL
    )
    BEGIN
        RAISERROR ('Machine ID does not exist in Machines table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_exercise_id INT;
    SELECT @max_exercise_id = ISNULL(MAX(exercise_id), 0) FROM Exercises;
    SET @max_exercise_id = @max_exercise_id + 1;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum exercise_id plus 1
        DBCC CHECKIDENT ('Exercises', RESEED, @max_exercise_id);
    END;
END;
GO


-- Trigger to enforce constraints and adjust default values on the WorkoutPlans table upon insertion or updating
CREATE TRIGGER trg_Enforce_WorkoutPlan_Constraints
ON WorkoutPlans
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('active', 'inactive')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: active, inactive.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if creator_id exists in Users table
    IF EXISTS (
        SELECT i.creator_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.creator_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Creator ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_plan_id INT;
    SELECT @max_plan_id = ISNULL(MAX(plan_id), 0) FROM WorkoutPlans;
    SET @max_plan_id = @max_plan_id + 1;

    -- Set default values for missing or incorrect data
    UPDATE wp
    SET 
        plan_date = COALESCE(wp.plan_date, GETDATE()),
        status = COALESCE(wp.status, 'active')
    FROM WorkoutPlans wp
    JOIN inserted i ON wp.plan_id = i.plan_id;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum plan_id plus 1
        DBCC CHECKIDENT ('WorkoutPlans', RESEED, @max_plan_id);
    END;
END;
GO





-- Trigger to enforce constraints and adjust default values on the DietPlans table upon insertion or updating
CREATE TRIGGER trg_Enforce_DietPlan_Constraints
ON DietPlans
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check type constraint
    IF EXISTS (
        SELECT type
        FROM inserted
        WHERE type NOT IN ('omnivore', 'vegan', 'vegetarian', 'pescatarian', 'gluten-free', 'lactose-free')
    )
    BEGIN
        RAISERROR ('Invalid type! Type must be one of: omnivore, vegan, vegetarian, pescatarian, gluten-free, lactose-free.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('active', 'inactive')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: active, inactive.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if creator_id exists in Users table
    IF EXISTS (
        SELECT i.creator_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.creator_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Creator ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_diet_id INT;
    SELECT @max_diet_id = ISNULL(MAX(diet_id), 0) FROM DietPlans;
    SET @max_diet_id = @max_diet_id + 1;

    -- Set default values for missing or incorrect data
    UPDATE dp
    SET 
        plan_date = COALESCE(dp.plan_date, GETDATE()),
        status = COALESCE(dp.status, 'active')
    FROM DietPlans dp
    JOIN inserted i ON dp.diet_id = i.diet_id;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum diet_id plus 1
        DBCC CHECKIDENT ('DietPlans', RESEED, @max_diet_id);
    END;
END;
GO




-- Trigger to enforce constraints on the DietPlanUsage table upon insertion or updating
CREATE TRIGGER trg_Enforce_DietPlanUsage_Constraints
ON DietPlanUsage
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if member_id exists in Users table
    IF EXISTS (
        SELECT member_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.member_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Member ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if diet_id exists in DietPlans table
    IF EXISTS (
        SELECT d.diet_id
        FROM inserted AS i
        LEFT JOIN DietPlans AS d ON i.diet_id = d.diet_id
        WHERE d.diet_id IS NULL
    )
    BEGIN
        RAISERROR ('Diet ID does not exist in DietPlans table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if creator_id exists in Users table
    IF EXISTS (
        SELECT creator_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.creator_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Creator ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_usage_id INT;
    SELECT @max_usage_id = ISNULL(MAX(usage_id), 0) FROM DietPlanUsage;
    SET @max_usage_id = @max_usage_id + 1;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum usage_id plus 1
        DBCC CHECKIDENT ('DietPlanUsage', RESEED, @max_usage_id);
    END;
END;
GO



-- Trigger to enforce constraints on the MemberPlanUsage table upon insertion or updating
CREATE TRIGGER trg_Enforce_MemberPlanUsage_Constraints
ON MemberPlanUsage
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if member_id exists in Users table
    IF EXISTS (
        SELECT member_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.member_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Member ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if plan_id exists in WorkoutPlans table
    IF EXISTS (
        SELECT wp.plan_id
        FROM inserted AS i
        LEFT JOIN WorkoutPlans AS wp ON i.plan_id = wp.plan_id
        WHERE wp.plan_id IS NULL
    )
    BEGIN
        RAISERROR ('Plan ID does not exist in WorkoutPlans table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if creator_id exists in Users table
    IF EXISTS (
        SELECT creator_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.creator_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Creator ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_usage_id INT;
    SELECT @max_usage_id = ISNULL(MAX(usage_id), 0) FROM MemberPlanUsage;
    SET @max_usage_id = @max_usage_id + 1;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum usage_id plus 1
        DBCC CHECKIDENT ('MemberPlanUsage', RESEED, @max_usage_id);
    END;
END;
GO

-- Trigger to enforce constraints and adjust default values on the Memberships table upon insertion or updating
CREATE TRIGGER trg_Enforce_Membership_Constraints
ON Memberships
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check membership_type constraint
    IF EXISTS (
        SELECT membership_type
        FROM inserted
        WHERE membership_type NOT IN ('monthly', 'yearly')
    )
    BEGIN
        RAISERROR ('Invalid membership type! Membership type must be either "monthly" or "yearly".', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check status constraint
    IF EXISTS (
        SELECT status
        FROM inserted
        WHERE status NOT IN ('active', 'expired', 'cancelled')
    )
    BEGIN
        RAISERROR ('Invalid status! Status must be one of: active, expired, cancelled.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if member_id exists in Users table
    IF EXISTS (
        SELECT i.member_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.member_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Member ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if gym_id exists in Gyms table
    IF EXISTS (
        SELECT i.gym_id
        FROM inserted AS i
        LEFT JOIN Gyms AS g ON i.gym_id = g.gym_id
        WHERE g.gym_id IS NULL
    )
    BEGIN
        RAISERROR ('Gym ID does not exist in Gyms table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_membership_id INT;
    SELECT @max_membership_id = ISNULL(MAX(membership_id), 0) FROM Memberships;
    SET @max_membership_id = @max_membership_id + 1;

    -- Set default values for missing or incorrect data
    UPDATE m
    SET 
        status = COALESCE(m.status, 'active')
    FROM Memberships m
    JOIN inserted i ON m.membership_id = i.membership_id;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum membership_id plus 1
        DBCC CHECKIDENT ('Memberships', RESEED, @max_membership_id);
    END;
END;
GO


-- Trigger to enforce constraints on the WorkoutExercises table upon insertion or updating
CREATE TRIGGER trg_Enforce_WorkoutExercise_Constraints
ON WorkoutExercises
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if workout_plan_id exists in WorkoutPlans table
    IF EXISTS (
        SELECT i.workout_plan_id
        FROM inserted AS i
        LEFT JOIN WorkoutPlans AS wp ON i.workout_plan_id = wp.plan_id
        WHERE wp.plan_id IS NULL
    )
    BEGIN
        RAISERROR ('Workout Plan ID does not exist in WorkoutPlans table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if exercise_id exists in Exercises table
    IF EXISTS (
        SELECT i.exercise_id
        FROM inserted AS i
        LEFT JOIN Exercises AS e ON i.exercise_id = e.exercise_id
        WHERE e.exercise_id IS NULL
    )
    BEGIN
        RAISERROR ('Exercise ID does not exist in Exercises table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

   
END;
GO


-- Trigger to enforce constraints on the Meals table upon insertion or updating
CREATE TRIGGER trg_Enforce_Meal_Constraints
ON Meals
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Check if calories are non-negative
        IF EXISTS (
            SELECT *
            FROM inserted AS i
            WHERE i.calories < 0
        )
        BEGIN
            RAISERROR ('Calories must be non-negative.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        -- Check if protein, carbs, fat, and fiber are non-negative
        IF EXISTS (
            SELECT *
            FROM inserted AS i
            WHERE i.protein < 0 OR i.carbs < 0 OR i.fat < 0 OR i.fiber < 0
        )
        BEGIN
            RAISERROR ('Protein, carbs, fat, and fiber must be non-negative.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        -- Adjust identity seed
        DECLARE @max_meal_id INT;
        SELECT @max_meal_id = ISNULL(MAX(meal_id), 0) FROM Meals;
        SET @max_meal_id = @max_meal_id + 1;

        -- Set identity seed to the maximum meal_id plus 1
        DBCC CHECKIDENT ('Meals', RESEED, @max_meal_id);
    END;
END;
GO


-- Trigger to enforce constraints and adjust default values on the Appointments table upon insertion or updating
CREATE TRIGGER trg_Enforce_Appointment_Constraints
ON Appointments
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if trainer_id exists in Trainers table
    IF EXISTS (
        SELECT i.trainer_id
        FROM inserted AS i
        LEFT JOIN Trainers AS t ON i.trainer_id = t.trainer_id
        WHERE t.trainer_id IS NULL
    )
    BEGIN
        RAISERROR ('Trainer ID does not exist in Trainers table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if member_id exists in Users table
    IF EXISTS (
        SELECT i.member_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.member_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Member ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if appointment_date is in the future
    IF EXISTS (
        SELECT appointment_date
        FROM inserted AS i
        WHERE i.appointment_date < GETDATE()
    )
    BEGIN
        RAISERROR ('Appointment date must be in the future.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if end_time is after start_time
    IF EXISTS (
        SELECT start_time, end_time
        FROM inserted AS i
        WHERE i.end_time <= i.start_time
    )
    BEGIN
        RAISERROR ('End time must be after start time.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_appointment_id INT;
    SELECT @max_appointment_id = ISNULL(MAX(appointment_id), 0) FROM Appointments;
    SET @max_appointment_id = @max_appointment_id + 1;

    -- Set default status for missing or incorrect data
    UPDATE a
    SET 
        status = COALESCE(a.status, 'pending')
    FROM Appointments a
    JOIN inserted i ON a.appointment_id = i.appointment_id;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum appointment_id plus 1
        DBCC CHECKIDENT ('Appointments', RESEED, @max_appointment_id);
    END;
END;
GO


-- Trigger to enforce constraints on the TrainerFeedback table upon insertion or updating
CREATE TRIGGER trg_Enforce_TrainerFeedback_Constraints
ON TrainerFeedback
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if member_id exists in Users table
    IF EXISTS (
        SELECT member_id
        FROM inserted AS i
        LEFT JOIN Users AS u ON i.member_id = u.user_id
        WHERE u.user_id IS NULL
    )
    BEGIN
        RAISERROR ('Member ID does not exist in Users table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if trainer_id exists in Trainers table
    IF EXISTS (
        SELECT t.trainer_id
        FROM inserted AS i
        LEFT JOIN Trainers AS t ON i.trainer_id = t.trainer_id
        WHERE t.trainer_id IS NULL
    )
    BEGIN
        RAISERROR ('Trainer ID does not exist in Trainers table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check rating constraint
    IF EXISTS (
        SELECT rating
        FROM inserted AS i
        WHERE i.rating < 0 OR i.rating > 5
    )
    BEGIN
        RAISERROR ('Rating must be between 0 and 5.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    DECLARE @max_feedback_id INT;
    SELECT @max_feedback_id = ISNULL(MAX(feedback_id), 0) FROM TrainerFeedback;
    SET @max_feedback_id = @max_feedback_id + 1;

    -- Check if there are any new inserted records
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Set identity seed to the maximum feedback_id plus 1
        DBCC CHECKIDENT ('TrainerFeedback', RESEED, @max_feedback_id);
    END;
END;
GO




-- Trigger to enforce constraints on the DietPlanMeals table upon insertion or updating
CREATE TRIGGER trg_Enforce_DietPlanMeals_Constraints
ON DietPlanMeals
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if diet_id exists in DietPlans table
    IF EXISTS (
        SELECT d.diet_id
        FROM inserted AS i
        LEFT JOIN DietPlans AS d ON i.diet_id = d.diet_id
        WHERE d.diet_id IS NULL
    )
    BEGIN
        RAISERROR ('Diet ID does not exist in DietPlans table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Check if meal_id exists in Meals table
    IF EXISTS (
        SELECT m.meal_id
        FROM inserted AS i
        LEFT JOIN Meals AS m ON i.meal_id = m.meal_id
        WHERE m.meal_id IS NULL
    )
    BEGIN
        RAISERROR ('Meal ID does not exist in Meals table.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Adjust identity seed
    -- (Note: No need to adjust identity seed for this table as it doesn't have an identity column)

END;
GO


SELECT * FROM DietPlanUsage;
SELECT * FROM Trainers;
SELECT * FROM Users;
SELECT * FROM Gyms;
SELECT * FROM Memberships;
SELECT * FROM Machines;
SELECT * FROM Exercises;
SELECT * FROM WorkoutPlans;
SELECT * FROM WorkoutExercises;
SELECT * FROM MemberPlanUsage;
SELECT * FROM DietPlanUsage;
SELECT * FROM DietPlans;
SELECT * FROM DietPlanMeals;
SELECT * FROM Meals;
SELECT * FROM Appointments;
SELECT * FROM TrainerFeedback;


SELECT COUNT(DISTINCT m.member_id) AS member_count
FROM Memberships m
JOIN MemberPlanUsage mp ON m.member_id = mp.member_id
JOIN WorkoutPlans wp ON mp.plan_id = wp.plan_id
JOIN WorkoutExercises we ON wp.plan_id = we.workout_plan_id
JOIN Exercises e ON we.exercise_id = e.exercise_id
JOIN Machines mach ON e.machine_id = mach.machine_id
WHERE m.gym_id = 1
    AND wp.plan_date = CONVERT(DATE, GETDATE())  
    AND mach.machine_name = 'Leg Press'; 

